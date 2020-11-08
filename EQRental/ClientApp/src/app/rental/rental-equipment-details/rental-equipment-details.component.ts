import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Equipment } from '../equipment.model';
import { RentalService } from '../rental.service';

@Component({
  selector: 'app-rental-equipment',
  templateUrl: './rental-equipment-details.component.html',
  styleUrls: ['./rental-equipment-details.component.css']
})
export class RentalEquipmentDetailsComponent implements OnInit {

  id: number;
  equipment: Equipment;

  constructor(private rentalService: RentalService,
    private route: ActivatedRoute,
    private router: Router) {}

  ngOnInit() {
    this.route.params.subscribe(
      (params: Params) => {
        this.id = +params['id'];
        this.rentalService.getRentalEquipment(this.id).subscribe(
          (equipmentData) => {
            this.equipment = equipmentData;
          }
        );
      }
    );
  }

  onClickBack() {
    this.router.navigate(['../'], {relativeTo: this.route});
  }

  onClickRent() {
    this.router.navigate(['rent'], {relativeTo: this.route});
  }
}
