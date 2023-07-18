import { HttpClient, HttpHeaderResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { Observable, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) { }

  getMembers() {
    if(this.members.length > 0 ) return of(this.members);          //for not loading page or save the state        
    // let headers = this.getHeaders();                                 //, { headers }
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members =>{
        this.members = members
        return members;
      })
    )
  }

  getMember(username: string) {
    const member = this.members.find(x => x.userName == username) 
    if(member) return of(member);
    // let headers = this.getHeaders();
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member : Member){
    return this.http.put(this.baseUrl + 'users',member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index], ...member}
      })
    )
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
