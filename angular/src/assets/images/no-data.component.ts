import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-no-data',
  templateUrl: './no-data.component.html',
  styleUrls: ['./no-data.component.scss'],
})
export class NoDataComponent implements OnInit {
  @Input() msg: string = 'no_data_available_msg';
  @Input() buttonLabel: string = 'add_new_data';
  @Input() routePath: string = '/';
  constructor() {}

  ngOnInit(): void {}
}
