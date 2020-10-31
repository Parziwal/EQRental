import { Address } from '../shared/models/address.model';
import { User } from '../shared/models/user.model';
import { EquipmentOverview } from '../shared/models/equipment-overview.model';

export interface Rental {
    id: number;
    equipment: EquipmentOverview;
    owner: User;
    address: Address;
    status: string;
    startDate: Date;
    endDate: Date;
    orderDate: Date;
    paymentMethod: string;
}
