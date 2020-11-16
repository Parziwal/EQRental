import { Address } from '../models/address.model';
import { User } from '../models/user.model';

export interface Rental {
  id: number;
  rentler: User;
  status: string;
  address: Address;
  startDate: Date;
  endDate: Date;
  orderDate: Date;
  paymentMethod: string;
}

/*
 *
 *
        <p id="currency">Name: {{rental?.rentler.name}}</p>
        <small>Start Date: {{ rental?.startDate | date:'medium' }}</small>
        <p id="currency">Payment Method: {{ rental?.paymentMethod}}</p>
        <p id="currency">Status: {{ rental?.status}}</p>
 */ 
