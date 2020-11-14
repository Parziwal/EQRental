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
  private equipments: EquipmentOverview[] = [];
  private equipmentsUpdate = new Subject<{ equipments: EquipmentOverview[] }>();
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
    return this.http.post<EquipmentPost>(this.apiUrl, equipmentData);
  }
}
