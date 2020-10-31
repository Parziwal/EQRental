import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { RentalOverview } from './rental-overview.model';
import { errorHandler } from '../error/error-handler';
import { Rental } from './rental.model';

@Injectable({
    providedIn: 'root'
})
export class RentedEquipmentsService {
    private rentals: RentalOverview[] = [];
    private rentalsUpdate = new Subject<{rentals: RentalOverview[]}>();
    private apiUrl = environment.apiUrl + 'rentedequipments/';

    constructor(private http: HttpClient) {}

    getRentals() {
        return this.http.get<RentalOverview[]>(this.apiUrl)
        .pipe(catchError(errorHandler), tap((rentalsData) => {
            this.rentals = rentalsData;
            this.rentalsUpdate.next({rentals: this.rentals.slice()});
        }));
    }

    getRentalsUpdateListener() {
        return this.rentalsUpdate.asObservable();
    }

    getRental(id: number) {
        return this.http.get<Rental>(this.apiUrl + id);
    }

    changeStatusTo(status: string, id: number) {
        let queryParams = new HttpParams();
        queryParams = queryParams.append('status', status);
        return this.http.put(this.apiUrl + id, {}, {
            params: queryParams
        });
    }

    getNextStatus(status: string) {
        if (status === 'PROCESSING' || status === 'PREPARING TO SHIP') {
            return 'CANCELED';
        } else if (status === 'UNDER DELIVERING') {
            return 'DELIVERED';
        }
    }
}
