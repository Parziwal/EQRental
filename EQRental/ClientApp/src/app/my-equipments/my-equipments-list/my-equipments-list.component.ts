import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { EquipmentOverview } from '../../shared/models/equipment-overview.model';
import { MyEquipmentsService } from '../my-equipments.service';

@Component({
  selector: 'app-my-equipments-list',
  templateUrl: './my-equipments-list.component.html',
  styleUrls: ['./my-equipments-list.component.css']
})
export class MyEquipmentsListComponent implements OnInit {
  equipments: EquipmentOverview[] = [];
  private equipmentSub: Subscription;

  constructor(private myEquipmentsService: MyEquipmentsService) { }

  ngOnInit() {
    this.myEquipmentsService.getEquipments().subscribe(
      (equipmentsData: EquipmentOverview[]) => {
        this.equipments = equipmentsData;
      }
    );
  }
}

