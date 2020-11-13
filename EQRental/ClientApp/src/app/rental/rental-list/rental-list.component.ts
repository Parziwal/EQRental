import { Component, OnInit } from '@angular/core';
import { EquipmentOverview } from 'src/app/shared/models/equipment-overview.model';
import { RentalService } from '../rental.service';

@Component({
  selector: 'app-rental-list',
  templateUrl: './rental-list.component.html',
  styleUrls: ['./rental-list.component.css']
})
export class RentalListComponent implements OnInit {

  equipments: EquipmentOverview[];

  constructor(private rentalService: RentalService) { }

  ngOnInit() {
    this.rentalService.getAllRentalEquipments().subscribe(
      (equipmentsData: EquipmentOverview[]) => {
        this.equipments = equipmentsData;
      }
    );

    this.rentalService.filterChanged.subscribe(
      (categories: string[]) => {
        if (categories.length === 0) {
          this.rentalService.getAllRentalEquipments().subscribe(
            (equipmentsData: EquipmentOverview[]) => {
              this.equipments = equipmentsData;
            }
          );
        } else {
          this.rentalService.getRentalEquipmentsByCategory(categories).subscribe(
            (equipmentsData: EquipmentOverview[]) => {
              this.equipments = equipmentsData;
            }
          );
        }
      }
    );
  }
}
