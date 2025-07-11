import { ListService, PagedResultDto } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, Validators } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { AuthorAppServicesService } from 'src/app/proxy/app-services/author';
import { AuthorDto } from 'src/app/proxy/iservices/author';
import { SharedModule } from 'src/app/shared/shared.module';
import { AppNoDataComponent } from "../../../../shared/Component/app-no-data/app-no-data.component";
import { debounceTime, Subject } from 'rxjs';
import { NgbDateNativeAdapter, NgbDateAdapter, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { PageChangedEvent, PaginationModule } from 'ngx-bootstrap/pagination';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-author',
  imports: [CommonModule, PaginationModule, SharedModule, NgxPaginationModule, FormsModule, AppNoDataComponent, NgbDatepickerModule],
  templateUrl: './author.component.html',
  styleUrl: './author.component.scss',
  providers: [ListService,
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }
  ],
})
export class AuthorComponent implements OnInit {
  pageNum = 1;
  pageSize = 7;
  totalCount = 0;
  searchText = '';
  author = { items: [], totalCount: 0 } as PagedResultDto<AuthorDto>;
  private searchTextChanged = new Subject<string>();
  isModalOpen = false; // add this line
  form: FormGroup; // add this line
  selectedAuthor = {} as AuthorDto;
  selectedAuthorId: string;
  totalPages: number = 0;
  constructor(
    public readonly list: ListService,
    private authorService: AuthorAppServicesService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.loadAuthors();
    this.searchTextChanged.pipe(
      debounceTime(300) // wait 300ms after user stops typing
    ).subscribe((searchTerm) => {
      this.searchText = searchTerm;
      this.pageNum = 1;
      this.loadAuthors();
    });
  }
  pageChanged(event: PageChangedEvent) {
    this.pageNum = event.page;
    this.loadAuthors();
    // Load data for the new page here
  }
  loadAuthors() {
    this.selectedAuthorId = ''; // Reset selected author ID when loading authors
    this.selectedAuthor = {} as AuthorDto; // Reset selected author object
    this.selectedAuthorId = '';
    this.selectedAuthor = {} as AuthorDto;
    this.authorService.getAllByInput({
      skipCount: (this.pageNum - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      filter: this.searchText
    }).subscribe(result => {
      this.author = result;
      this.totalCount = result.totalCount;
      this.totalPages = Math.ceil(this.totalCount / this.pageSize);
    });
  }
  onSearch(): void {
    this.pageNum = 1; // reset to first page when filtering
    this.loadAuthors();
  }
  onSearchTextChange(text: string): void {
    this.searchTextChanged.next(text);
  }
  createAuthor() {
    this.selectedAuthor = {} as AuthorDto;
    this.buildForm(); // add this line
    this.isModalOpen = true;
  }
  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedAuthor.name || '', Validators.required],
      surname: [this.selectedAuthor.surname || '', Validators.required],
      bio: [this.selectedAuthor.bio || null, Validators.required],
      picture: [this.selectedAuthor.picture || null, Validators.required],
      birthDate: [this.selectedAuthor.birthDate ? new Date(this.selectedAuthor.birthDate) : null, Validators.required],
      deathDate: [this.selectedAuthor.deathDate ? new Date(this.selectedAuthor.deathDate) : null, null],
    });
  }
  save() {
    if (this.form.invalid) {
      return;
    }
    if (this.selectedAuthor.id) {
      this.form.addControl('id', new FormControl(this.selectedAuthor.id || '', null));
      this.form.patchValue({ id: this.selectedAuthor.id });
    }
    const request = this.selectedAuthor.id
      ? this.authorService.updateByInput(this.form.value)
      : this.authorService.insertByInput(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.loadAuthors();
      this.toastr.success('Author saved successfully!', 'Success');
    });
  }
  deleteAuthor() {
    const id = this.selectedAuthorId; // Use the stored selected author ID
    if (!id) {
      return; // If no author is selected, do nothing
    }
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.authorService.deleteByIdGuidById(id).subscribe(() => {
          this.pageNum = 1; // Reset to the first page after deletion
          this.loadAuthors()
          this.selectedAuthorId = '';
          this.isModalOpen = false;
          this.form.reset();
          this.toastr.success('Author Deleted successfully!', 'Success');

        });
      }
    });
  }
  editAuthor(id: string) {
    this.authorService.getByIdGuidById(id).subscribe((author) => {
      this.selectedAuthor = author;
      console.log('selectedAuthor=',this.selectedAuthor);
      this.buildForm();
      this.isModalOpen = true;
      this.selectedAuthorId = id; // Store the selected author ID 
    });
  }

}