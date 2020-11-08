import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Address } from '../models/address.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AddressService {

  private apiUrl = environment.apiUrl + 'useraddresses/';

  constructor(private http: HttpClient) { }

  getAddresses() {
    return this.http.get<Address[]>(this.apiUrl);
  }
}
