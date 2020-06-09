import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


import { environment } from 'src/environments/environment';
import { UsersDataModel } from 'src/app/models/UsersDataModel';


@Injectable({
  providedIn: 'root'
})
export class UsersDataService {
  private readonly endpoint = 'UserData';
  constructor(private http: HttpClient) { }

  public SaveUserData(userDataModel: UsersDataModel): Observable<number> {
    
    console.log(userDataModel);
    return this.http.post<number>(`${environment.dataUrl}/${this.endpoint}/SaveUserData`, userDataModel);
  }

  public GetUsersData(UserId: number, userName: string, searchString: string): Observable<UsersDataModel[]> {
    return this.http.get<UsersDataModel[]>(`${environment.dataUrl}/${this.endpoint}/GetUsersData/${UserId}/${userName}/${searchString}`, {});
  }


}
