import { Injectable } from '@angular/core';
import { Customer } from './customer.model';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  constructor(private http: HttpClient) {
  }
  readonly _baseUrl = "https://localhost:44391/api/customer";
  formData: Customer = new Customer();
  customers: Customer[] = [];

  postMember() {
    return this.http.post<Customer>(this._baseUrl, this.formData);
  }
  
  getCustomers()  {
    return this.http.get<Customer[]>(this._baseUrl);
  }

  getCustomer(id: number) {
    const url = '${this._baseUrl}/${id}';
    return this.http.get<Customer[]>(url);
  }

}

