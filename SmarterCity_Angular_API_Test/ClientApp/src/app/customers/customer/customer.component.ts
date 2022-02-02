import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styles: [
  ]
})
export class CustomerComponent implements OnInit {
  constructor(public service: CustomerService) { }

  ngOnInit(): void {
     this.service.getCustomers().subscribe((response: Customer[]) => {
       this.service.customers = response;
     }, err => {
       console.log(err.message);
     });
  }

  populateForm(selectedRecord: Customer) {
    this.service.formData = Object.assign({}, selectedRecord);
  }

}


