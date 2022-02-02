import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { CustomerService } from '../customer.service';
import { Customer } from '../customer';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource, } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { SelectionModel } from '@angular/cdk/collections';


interface Country {
  CountryId: string;
  CountryName: string;
}

interface State {
  StateId: string;
  StateName: string;
  CountryId: string;
}

interface City {
  Cityid: string;
  CityName: string;
  StateId: string;
  CountryId: string;
}

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})

export class CustomerComponent implements OnInit {
  dataSaved = false;
  customerForm: any;
  allCustomers: Observable<Customer[]>;
  dataSource: MatTableDataSource<Customer>;
  selection = new SelectionModel<Customer>(true, []);
  customerIdUpdate = null;
  massage = null;
  allCountry: Observable<Country[]>;
  allState: Observable<State[]>;
  allCity: Observable<City[]>;
  CountryId = null;
  StateId = null;
  CityId = null;
  SelectedDate = null;
  isMale = true;
  isFeMale = false;
  horizontalPosition: MatSnackBarHorizontalPosition = 'center';
  verticalPosition: MatSnackBarVerticalPosition = 'bottom';
  displayedColumns: string[] = ['select', 'FirstName', 'LastName', 'MobileNumber', 'Email', 'Country', 'State', 'City', 'Address'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private formbulider: FormBuilder, private CustomerService: CustomerService, private _snackBar: MatSnackBar, public dialog: MatDialog) {
    this.CustomerService.getAllCustomer().subscribe(data => {
      this.dataSource = new MatTableDataSource(data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  ngOnInit() {
    this.customerForm = this.formbulider.group({
      FirstName: ['', [Validators.required]],
      LastName: ['', [Validators.required]],
      Email: ['', [Validators.required]],
      MobileNumber: ['', [Validators.required]],
      Address: ['', [Validators.required]],
      Country: ['', [Validators.required]],
      State: ['', [Validators.required]],
      City: ['', [Validators.required]]
    });
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = !!this.dataSource && this.dataSource.data.length;
    return numSelected === numRows;
  }

 /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ? this.selection.clear() : this.dataSource.data.forEach(r => this.selection.select(r));
  }
  /** The label for the checkbox on the passed row */
  checkboxLabel(row: Customer): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.Id + 1}`;
  }
 

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  loadAllCustomers() {
    this.CustomerService.getAllCustomer().subscribe(data => {
      this.dataSource = new MatTableDataSource(data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }
  onFormSubmit() {
    this.dataSaved = false;
    const customer = this.customerForm.value;
    this.CreateCustomer(customer);
    this.customerForm.reset();
  }

  loadCustomerToEdit(customerId: string) {
    this.CustomerService.getCustomerById(customerId).subscribe(customer => {
      this.massage = null;
      this.dataSaved = false;
      this.customerIdUpdate = customer.Id;
      this.customerForm.controls['FirstName'].setValue(customer.FirstName);
      this.customerForm.controls['LastName'].setValue(customer.LastName);
      this.customerForm.controls['Email'].setValue(customer.Email);
      this.customerForm.controls['Address'].setValue(customer.Address);
    });

  }
  CreateCustomer(customer: Customer) {
    if (this.customerIdUpdate == null) {

      this.CustomerService.createCustomer(customer).subscribe(
        () => {
          this.dataSaved = true;
          this.SavedSuccessful(1);
          this.loadAllCustomers();
          this.customerIdUpdate = null;
          this.customerForm.reset();
        }
      );
    }
  }

  resetForm() {
    this.customerForm.reset();
    this.massage = null;
    this.dataSaved = false;
    this.isMale = true;
    this.isFeMale = false;
    this.loadAllCustomers();
  }

  SavedSuccessful(isUpdate) {
    if (isUpdate == 0) {
      this._snackBar.open('Record Updated Successfully!', 'Close', {
        duration: 2000,
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
    } 
    else if (isUpdate == 1) {
      this._snackBar.open('Record Saved Successfully!', 'Close', {
        duration: 2000,
        horizontalPosition: this.horizontalPosition,
        verticalPosition: this.verticalPosition,
      });
    }
  }
}
