import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { EquipmentOverview } from '../shared/models/equipment-overview.model';
import { Equipment } from './equipment.model';
import { RentalOrder } from './rental-order';

@Injectable({
  providedIn: 'root'
})
export class RentalService {

  private apiUrl = environment.apiUrl + 'equipment/';

  constructor(private http: HttpClient) { }

  getRentalEquipments() {
    return this.http.get<EquipmentOverview[]>(this.apiUrl);
  }

  getRentalEquipment(id: number) {
    return this.http.get<Equipment>(this.apiUrl + id);
  }

  postRental(rental: RentalOrder) {
    return this.http.post<RentalOrder>(this.apiUrl, rental);
  }
}
