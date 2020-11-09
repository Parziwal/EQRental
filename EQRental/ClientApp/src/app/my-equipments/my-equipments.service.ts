import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { errorHandler } from '../error/error-handler';
import { EquipmentOverview } from '../shared/models/equipment-overview.model';
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
}
