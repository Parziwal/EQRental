import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private apiUrl = environment.apiUrl + 'payments/';

  constructor(private http: HttpClient) { }

  getPayments() {
    return this.http.get<string[]>(this.apiUrl);
  }
}
