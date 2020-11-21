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
