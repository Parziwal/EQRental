export class RentalOrder {
    constructor(public equipmentId: number,
    public addressId: number,
    public startDate: Date,
    public endDate: Date,
    public paymentMethod: string) {}
}
