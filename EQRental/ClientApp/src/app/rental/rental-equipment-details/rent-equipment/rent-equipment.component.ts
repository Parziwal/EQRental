import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Address } from 'src/app/shared/models/address.model';
import { AddressService } from 'src/app/shared/services/address.service';
import { PaymentService } from 'src/app/shared/services/payment.service';
import { Equipment } from '../../equipment.model';
import { RentalService } from '../../rental.service';

@Component({
  selector: 'app-rent-equipment',
  templateUrl: './rent-equipment.component.html',
  styleUrls: ['./rent-equipment.component.css']
})
export class RentEquipmentComponent implements OnInit {
  id: number;
  equipment: Equipment;
  addresses: Address[];
  payments: string[];
  rentalForm: FormGroup;

  constructor(private route: ActivatedRoute, private router: Router,
    private rentalService: RentalService,
    private addressService: AddressService,
    private paymentService: PaymentService) { }

  ngOnInit(): void {
    this.route.params.subscribe(
      (params: Params) => {
        this.id = +params['id'];
        this.initForm();
        this.rentalService.getRentalEquipment(this.id).subscribe(
          (equipmentData) => {
            this.equipment = equipmentData;
          }
        );
      }
    );
    this.addressService.getAddresses().subscribe(
      (addressData: Address[]) => {
        this.addresses = addressData;
      }
    );
    this.paymentService.getPayments().subscribe(
      (paymentData: string[]) => {
        this.payments = paymentData;
      }
    );
  }

  private initForm() {
    this.rentalForm = new FormGroup({
      addressId: new FormControl(null, Validators.required),
      startDate: new FormControl(null, Validators.required),
      endDate: new FormControl(null, Validators.required),
      paymentMethod: new FormControl(null, Validators.required),
    });
  }

  onSubmit() {
    const rental = {equipmentId: this.id, ...this.rentalForm.value, addressId: +this.rentalForm.value.addressId};
    this.rentalService.postRental(rental).subscribe();
    this.onCancel();
  }

  onCancel() {
    this.router.navigate(['../'], {relativeTo: this.route});
  }
}
