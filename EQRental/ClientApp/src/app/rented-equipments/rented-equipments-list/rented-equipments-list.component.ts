import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { RentalOverview } from '../rental-overview.model';
import { RentedEquipmentsService } from '../rented-equipments.service';

@Component({
  selector: 'app-rented-equipments-list',
  templateUrl: './rented-equipments-list.component.html',
  styleUrls: ['./rented-equipments-list.component.css']
})
export class RentedEquipmentsListComponent implements OnInit, OnDestroy {
  rentals: RentalOverview[] = [];
  private rentalSub: Subscription;

  constructor(private rentedEquipmentsService: RentedEquipmentsService) { }

  ngOnInit() {
    this.rentedEquipmentsService.getRentals().subscribe();
    this.rentalSub = this.rentedEquipmentsService.getRentalsUpdateListener().subscribe(
      (rentalData: {rentals: RentalOverview[]}) => {
        this.rentals = rentalData.rentals;
      }
    );
  }

  ngOnDestroy() {
    this.rentalSub.unsubscribe();
  }
}
