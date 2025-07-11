import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-no-data',
  imports: [],
  templateUrl: './app-no-data.component.html',
  styleUrl: './app-no-data.component.scss'
})
export class AppNoDataComponent implements OnInit {
  @Input() msg: string = 'no_data_available_msg';
  @Input() buttonLabel: string = 'add_new_data';
  @Input() routePath: string = '/';
  constructor() {}

  ngOnInit(): void {}
}