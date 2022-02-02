
import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../customer.service';
import { NgForm } from '@angular/forms';
import { Customer } from '../customer.model';
@Component({
  selector: 'app-customer-details',
  templateUrl: './customer-details.component.html',
  styles: [
  ]
})
export class CustomerDetailsComponent implements OnInit {
  constructor(public service: CustomerService) {
  }
  ngOnInit(): void {
  }

  onSubmit() {
    console.log('onSubmit');
    this.insertRecord();
  }

  insertRecord() {
    this.service.postMember().subscribe(
      res => {
        this.service.formData = new Customer();
        this.refresh();
      },
      err => {
        console.log(err);
      }
    );
  }

  refresh() {
    this.service.getCustomers().subscribe((response: Customer[]) => {
      this.service.customers = response;
    }, err => {
      console.log(err.message);
    });
  }
}
