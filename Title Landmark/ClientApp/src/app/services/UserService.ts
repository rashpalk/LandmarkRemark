import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


import { environment } from 'src/environments/environment';
import { UserModel } from 'src/app/models/UserModel';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly endpoint = 'User';
  constructor(private http: HttpClient) { }

  public RegisterUser(userModel: UserModel): Observable<number> {
    
    console.log(userModel);
    return this.http.post<number>(`${environment.dataUrl}/${this.endpoint}/Register`, userModel);
  }

  public GetUser(userId: number, userName: string, password: string): Observable<UserModel> {
    return this.http.get<UserModel>(`${environment.dataUrl}/${this.endpoint}/GetUser/${userId}/${userName}/${password}`, {});
  }


}
