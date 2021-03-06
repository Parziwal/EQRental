import { User } from '../shared/models/user.model';

export interface Equipment {
    id: number;
    name: string;
    details: string;
    imagePath: string;
    pricePerDay: number;
    avalilable: boolean;
    category: string;
    owner: User;
}