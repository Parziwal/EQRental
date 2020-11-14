import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { RentalComponent } from 'src/app/rental/rental.component';
import { Rental } from '../rental.model';
import { RentedEquipmentsService } from '../rented-equipments.service';

@Component({
  selector: 'app-rented-equipment-details',
  templateUrl: './rented-equipment-details.component.html',
  styleUrls: ['./rented-equipment-details.component.css']
})
export class RentedEquipmentDetailsComponent implements OnInit {
  id: number;
  rental: Rental;
  subTotal: number;
  statusBtnLabel: string;
  nextStatus: string;

  constructor(private rentedEquipmetsService: RentedEquipmentsService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.route.params.subscribe(
      (params: Params) => {
        this.id = +params['id'];
        this.rentedEquipmetsService.getRental(this.id).subscribe(
          (rentalData) => {
            this.rental = rentalData;

            const days = new Date(rentalData.endDate).getTime() - new Date(rentalData.startDate).getTime();
            this.subTotal = new Date(days).getDate() * rentalData.equipment.pricePerDay;

            this.setStatus(rentalData.status);
          }
        );
      }
    );
  }

  onClickBack() {
    this.router.navigate(['../'], {relativeTo: this.route});
  }

  onSetStatus() {
    this.rentedEquipmetsService.changeStatusTo(this.nextStatus, this.id).subscribe(
      response => {
        this.rental.status = this.nextStatus;
        this.statusBtnLabel = null;
        this.setStatus(this.nextStatus);
      }
    );
  }

  private setStatus(status: string) {
    this.nextStatus = this.rentedEquipmetsService.getNextStatus(status);
    if (this.nextStatus === 'CANCELED') {
      this.statusBtnLabel = 'Cancel rental';
    } else if (this.nextStatus === 'DELIVERED') {
      this.statusBtnLabel = 'Equipment arrived';
    }
  }
}
