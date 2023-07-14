import { HttpClient, HttpHeaderResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getMembers() {
    // let headers = this.getHeaders();
    return this.http.get<Member[]>(this.baseUrl + 'users');  //, { headers }
  }

  getMember(username: string) {
    // let headers = this.getHeaders();
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  // getHeaders() {
  //   const userString = localStorage.getItem('user');
  //   if (!userString) return;

  //   const user = JSON.parse(userString);

  //   let headers = new HttpHeaders();
  //   headers = headers.set("Authorization", 'Bearer ' + user.token);
  //   return headers;
  // }

  // getHttpOptions(): object {
  //   const userString = localStorage.getItem('user');
  //   if (!userString) return null;
  //   // if (!userString) return null;
  //   const user = JSON.parse(userString);
  //   return {
  //     headers: new HttpHeaders({
  //       Authorization: 'Bearer ' + user.token
  //     })
  //   }
  // }

  // const httpOptions ={
  //   Headers: new HttpHeaders({
  //     Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token

  //   })
  // }

}
