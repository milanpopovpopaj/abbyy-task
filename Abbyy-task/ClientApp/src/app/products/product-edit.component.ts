import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';

import { Product } from './product';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css']
})

export class ProductEditComponent implements OnInit {
  // the view title
  title: string;
  // the form model
  form: FormGroup;
  // the product object to edit or create
  product: Product;

  // the product object id, as fetched from the active route:
  // It's NULL when we're adding a new product,
  // and not NULL when we're editing an existing one.
  id?: number;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.form = new FormGroup({
      name: new FormControl(''),
      price: new FormControl(''),
      description: new FormControl('')
    });
    this.loadData();
  }

  loadData() {
    // retrieve the ID from the 'id'
    this.id = +this.activatedRoute.snapshot.paramMap.get('id');
    if (this.id) {
      // EDIT MODE
      // fetch the product from the server
      var url = this.baseUrl + "api/Products/" + this.id;
      this.http.get<Product>(url).subscribe(result => {
        this.product = result;
        this.title = "Edit - " + this.product.name;
        // update the form with the product value
        this.form.patchValue(this.product);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE
      this.title = "Create a new Product";
    }
  }

  onSubmit() {
    var product = (this.id) ? this.product : <Product>{};

    product.name = this.form.get("name").value;
    product.price = +this.form.get("price").value;
    product.description = this.form.get("description").value;

    if (this.id) {
      // EDIT mode
      var url = this.baseUrl + "api/Products/" + this.product.id;
      this.http
        .put<Product>(url, product)
        .subscribe(result => {
          console.log("Product " + product.id + " has been updated.");
          // go back to products view
          this.router.navigate(['/products']);
        }, error => console.error(error));
    }
    else {
      // ADD NEW mode
      var url = this.baseUrl + "api/Products";
      this.http
        .post<Product>(url, product)
        .subscribe(result => {
          console.log("Product " + result.id + " has been created.");
          // go back to products view
          this.router.navigate(['/products']);
        }, error => console.error(error));
    }
  }
}
