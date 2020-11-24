import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Rental } from 'src/app/shared/models/rental.model';
import { MyEquipmentsService } from '../../my-equipments.service';

@Component({
  selector: 'app-equipment-rental',
  templateUrl: './equipment-rental.component.html',
  styleUrls: ['./equipment-rental.component.css']
})
export class EquipmentRentalComponent implements OnInit {
  @Input() rental: Rental;
  @Input() equipmentId: number;
  @Input() pricePerDay: number;
  private statuses: string[] = ['PROCESSING', 'PREPARING TO SHIP', 'UNDER DELIVERING', 'DELIVERED'];
  subtotal: number;
  nextStatus: string;

  constructor(private myEquipmentsService: MyEquipmentsService) { }

  ngOnInit() {
    this.subtotal = this.getSubtotal();
    this.nextStatus = this.getNextStatus(this.rental.status);
  }

  private getSubtotal() {
    const days = new Date(this.rental.startDate).getTime() - new Date(this.rental.endDate).getTime();
    return new Date(days).getDate() * this.pricePerDay;
  }

  private getNextStatus(currentStatus: string) {
    const index = this.statuses.indexOf(currentStatus);
    if (index !== -1 && index !== status.length - 1) {
      return this.statuses[index + 1];
    } else {
      return null;
    }
  }

  onSetStatus(status: string) {
    this.myEquipmentsService.changeStatusTo(this.equipmentId, this.rental.id, status).subscribe(
      response => {
        this.rental.status = status;
        this.nextStatus = this.getNextStatus(status);
      }
    );
  }
}
