import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { GridParameters } from "src/app/components/grid/grid-parameters";
import { GridService } from "src/app/components/grid/grid.service";
import { User } from "src/app/models/user";

@Injectable({ providedIn: "root" })
export class AppUserService {
    constructor(
        private readonly http: HttpClient,
        private readonly gridService: GridService) { }

    add(user: User) {
        return this.http.post<number>("users", user);
    }

    delete(id: number) {
        return this.http.delete(`users/${id}`);
    }

    get(id: number) {
        return this.http.get<User>(`users/${id}`);
    }

    grid(parameters: GridParameters) {
        return this.gridService.get<User>(`users/grid`, parameters);
    }

    inactivate(id: number) {
        return this.http.patch(`users/${id}/inactivate`, {});
    }

    list() {
        return this.http.get<User[]>("users");
    }

    update(user: User) {
        return this.http.put(`users/${user.id}`, user);
    }
}
