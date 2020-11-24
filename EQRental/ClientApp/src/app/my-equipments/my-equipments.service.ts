import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { errorHandler } from '../error/error-handler';
import { EquipmentOverview } from '../shared/models/equipment-overview.model';
import { EquipmentPost } from './equipment-create/equipment-post';
import { Equipment } from './equipment.model';

@Injectable({
  providedIn: 'root'
})
export class MyEquipmentsService {
  private apiUrl = environment.apiUrl + 'ownequipments/';

  constructor(private http: HttpClient) { }

  getEquipments() {
    return this.http.get<EquipmentOverview[]>(this.apiUrl);
  }
  getEquipment(id: number) {
    return this.http.get<Equipment>(this.apiUrl + id);
  }

  addEquipment(newEquipment: EquipmentPost) {
    const equipmentData = new FormData();
    equipmentData.append('name', newEquipment.name);
    equipmentData.append('details', newEquipment.details);
    equipmentData.append('image', newEquipment.image);
    equipmentData.append('pricePerDay', newEquipment.pricePerDay.toString());
    equipmentData.append('category', newEquipment.category);
    return this.http.post<number>(this.apiUrl, equipmentData);
  }

  updateEquipment(id: number, equipment: EquipmentPost, image: File | string) {
    const equipmentData = new FormData();
    if (typeof(image) === 'object') {
      equipmentData.append('image', equipment.image);
    } else {
      equipmentData.append('image', null);
    }
    equipmentData.append('name', equipment.name);
    equipmentData.append('details', equipment.details);
    equipmentData.append('pricePerDay', equipment.pricePerDay.toString());
    equipmentData.append('category', equipment.category);
    return this.http.put(this.apiUrl + id, equipmentData);
  }

  deleteEquipment(id: number) {
    return this.http.delete(this.apiUrl + id);
  }

  changeStatusTo(equipmentId: number, rentalId: number, status: string) {
    const statusData = new FormData();
    statusData.append('status', String(status));
    return this.http.put(this.apiUrl + equipmentId + '/rentalstatus/' + rentalId, statusData);
  }

  changeAvailable(id: number, value: boolean) {
    const availableData = new FormData();
    availableData.append('available', String(value));
    return this.http.put(this.apiUrl + id + '/status', availableData);
  }
}
