import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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

  constructor(private myEquipmentsService: MyEquipmentsService,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.myEquipmentsService.getEquipments().subscribe(
      (equipmentsData: EquipmentOverview[]) => {
        this.equipments = equipmentsData;
      }
    );
  }

  onAddEquipment() {
    this.router.navigate(['create'], {relativeTo: this.route});
  }

  onEditEquipment(id: number) {
    this.router.navigate(['edit', id], {relativeTo: this.route});
  }

  onDeleteEquipment(id: number, index: number) {
    this.myEquipmentsService.deleteEquipment(id).subscribe(
      (response) => {
        this.equipments.splice(index, 1);
      }
    );
  }
}

