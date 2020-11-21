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
  private statuses: string[] = ['PROCESSING', 'PREPARING TO SHIP', 'UNDER DELIVERING', 'DELIVERED'];

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
          }
        );
      }
    );
  }

  getSubtotal(startDate: Date, endDate: Date) {
    const days = new Date(startDate).getTime() - new Date(endDate).getTime();
    return new Date(days).getDate() * this.equipment.pricePerDay;
  }

  getNextStatus(currentStatus: string) {
    const index = this.statuses.indexOf(currentStatus);
    if (index !== -1 && index !== status.length - 1) {
      return this.statuses[index + 1];
    } else {
      return null;
    }
  }

  onSetStatus(index: number, status: string) {
    this.myEquipmentsService.changeStatusTo(status, this.equipment.rentals[index].id).subscribe(
      response => {
        this.equipment.rentals[index].status = status;
      }
    );
  }

  onClickBack() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }
}

