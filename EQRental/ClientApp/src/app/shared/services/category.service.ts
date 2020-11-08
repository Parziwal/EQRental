import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private apiUrl = environment.apiUrl + 'categories/';

  constructor(private http: HttpClient) { }

  getCategories() {
    return this.http.get<string[]>(this.apiUrl);
  }
}
