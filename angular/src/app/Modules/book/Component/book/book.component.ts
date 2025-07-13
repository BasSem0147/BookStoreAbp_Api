import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, Validators } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { PageChangedEvent, PaginationModule } from 'ngx-bootstrap/pagination';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, Subject } from 'rxjs';
import { AuthorAppServicesService } from 'src/app/proxy/app-services/author';
import { BookAppServicesService } from 'src/app/proxy/app-services/book';
import { bookTypeOptions } from 'src/app/proxy/enums';
import { AuthorDto } from 'src/app/proxy/iservices/author';
import { BookDto } from 'src/app/proxy/iservices/book';
import { AppNoDataComponent } from 'src/app/shared/Component/app-no-data/app-no-data.component';
import { FileService } from 'src/app/shared/Services/file.service';
import { NotificationService } from 'src/app/shared/Services/notification.service';
import { SharedModule } from 'src/app/shared/shared.module';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-book',
  imports: [CommonModule, PaginationModule, SharedModule, FormsModule, AppNoDataComponent, NgbDatepickerModule],
  templateUrl: './book.component.html',
  styleUrl: './book.component.scss',
  standalone: true,
  providers: [ListService]
})
export class BookComponent implements OnInit {
  pageNum = 1;
  pageSize = 7;
  totalCount = 0;
  searchText = '';
  Books = { items: [], totalCount: 0 } as PagedResultDto<BookDto>;
  authors: AuthorDto[] = [];
  private searchTextChanged = new Subject<string>();
  isModalOpen = false; // add this line
  form: FormGroup; // add this line
  selectedBook = {} as BookDto;
  selectedBookId: string;
  totalPages: number = 0;
  selectedFile: File | null = null;
  uploadedImageUrl: string | null = null;
  bookType = bookTypeOptions;
  apiUrl = environment.apis.default.url; // Use the apiUrl from environment
  constructor(
    public readonly list: ListService,
    private bookService: BookAppServicesService,
    private fb: FormBuilder,
    private confirmation: ConfirmationService,
    private toastr: ToastrService,
    private fileService: FileService,
    private authService: AuthorAppServicesService,
    private notificationService: NotificationService
  ) { }

  ngOnInit() {
    this.sendMessage();
    this.getBooks();
    this.searchTextChanged.pipe(
      debounceTime(300) // wait 300ms after user stops typing
    ).subscribe((searchTerm) => {
      this.searchText = searchTerm;
      this.pageNum = 1;
      this.getBooks();
    });
  }
  pageChanged(event: PageChangedEvent) {
    this.pageNum = event.page;
    this.getBooks();
    // Load data for the new page here
  }
  getBooks() {
    this.selectedBookId = ''; // Reset selected author ID when loading authors
    this.selectedBook = {} as BookDto; // Reset selected author object
    this.selectedBook = {} as BookDto;
    this.bookService.getAllByInput({
      skipCount: (this.pageNum - 1) * this.pageSize,
      maxResultCount: this.pageSize,
      filter: this.searchText,
      sorting: 'creationTime desc'
    }).subscribe(result => {
      this.Books.items = result.items;
      console.log(this.Books);
      this.totalCount = result.totalCount;
      this.totalPages = Math.ceil(this.totalCount / this.pageSize);
    });
  }
  loadAuthors() {
    this.authService.getAllAutorFromCache().subscribe(result => {
      this.authors = result;
    });
  }
  onSearch(): void {
    this.pageNum = 1; // reset to first page when filtering
    this.getBooks();
  }
  onSearchTextChange(text: string): void {
    this.searchTextChanged.next(text);
  }
  createBook() {
    this.loadAuthors();
    this.isModalOpen = true;
    this.selectedBook = {} as BookDto;
    this.buildForm(); // add this line
  }
  buildForm() {
    this.form = this.fb.group({
      name: [this.selectedBook.name || '', [Validators.required, Validators.minLength(8)]],
      authorId: [this.selectedBook.authorId || '', [Validators.required]],
      type: [this.selectedBook.type || '', [Validators.required]],
      picture: [this.selectedBook.picture || '', []],
      publishDate: [this.selectedBook.publishDate?new Date(this.selectedBook.publishDate) : null, []],
      price: [this.selectedBook.price || '', [Validators.required]]
    });
  }
  save() {
    if (this.form.invalid) {
      return;
    }
    if (this.selectedBook.id) {
      this.form.addControl('id', new FormControl(this.selectedBook.id || '', null));
      this.form.patchValue({ id: this.selectedBook.id });
    }
    const rawForm = this.form.value;

  // ✅ Manually convert NgbDateStruct to ISO string
  const dp = rawForm.publishDate;
  const publishDate = new Date(dp.year, dp.month - 1, dp.day);

  // ✅ Create the payload with converted date
  const payload: any = {
    ...rawForm,
    publishDate: publishDate.toISOString()
  };
    const request = this.selectedBook.id
      ? this.bookService.updateByInput(payload)
      : this.bookService.insertByInput(payload);

    request.subscribe((res) => {
      console.log('Book saved successfully', res.id);
      if (this.selectedFile) {
        this.fileService.uploadFile('Books' + res.id, this.selectedFile).subscribe({
          next: (res) => {
            console.log('File uploaded successfully', res);
            setTimeout(() => {
              this.toastr.success('File saved successfully!', 'Success');
            }, 3000);
          },
          error: (err) => {
            setTimeout(() => {
              this.toastr.error('File Upload failed!', 'Error');
            }, 3000);
          }
        });
      }
      this.isModalOpen = false;
      this.form.reset();
      this.getBooks();
      this.toastr.success('Book saved successfully!', 'Success');
    });
  }
  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }
  sendMessage() {
  this.notificationService.sendMessage('Basem', 'Hello from Angular!');
}
}
