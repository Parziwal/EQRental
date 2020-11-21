import { Rental } from '../shared/models/rental.model';

export interface Equipment {
  id: number;
  name: string;
  details: string;
  imagePath: string;
  pricePerDay: number;
  avalilable: boolean;
  category: string;
  rentals: Array<Rental>;
}
