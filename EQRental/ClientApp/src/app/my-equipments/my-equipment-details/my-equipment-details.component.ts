import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Equipment } from '../equipment.model';
import { MyEquipmentsService } from '../my-equipments.service';

@Component({
  selector: 'app-my-equipment-details',
  templateUrl: './my-equipment-details.component.html',
  styleUrls: ['./my-equipment-details.component.css']
})
export class MyEquipmentsDetailsComponent implements OnInit {
  id: number;
  equipment: Equipment;
  isAvailable = true;

  constructor(private myEquipmentsService: MyEquipmentsService,
    private route: ActivatedRoute,
    private router: Router) {}

  ngOnInit() {
    this.route.params.subscribe(
      (params: Params) => {
        this.id = +params['id'];
        this.myEquipmentsService.getEquipment(this.id).subscribe(
          (equipmentData) => {
            this.equipment = equipmentData;
            this.isAvailable = this.equipment.available;
          }
        );
      }
    );
  }

  onAvailableChanged() {
    this.myEquipmentsService.changeAvailable(this.equipment.id, this.isAvailable).subscribe();
  }

  onClickBack() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }
}

