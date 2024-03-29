import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { JsonPipe } from '@angular/common';

@Injectable({           //decorator
  providedIn: 'root'
})
export class AccountService {
  //baseUrl = 'https://localhost:5001/api/';
  baseUrl = environment.apiUrl;
  
  private currentUserSource = new BehaviorSubject<User | null>(null);             //pending - an observable
  currentUser$ = this.currentUserSource.asObservable();                            //pending 

  constructor(private http: HttpClient) { }   

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
      map((response : User) =>{
        const user = response;
        if(user){
          // localStorage.setItem('user',JSON.stringify(user))  //setting item to the local storage
          // this.currentUserSource.next(user);
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl +'account/register',model).pipe(
      map(user => {
        if(user)
        {
          this.setCurrentUser(user);
        }
        // return user;        //not want 
      })
    )
  }

  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user',JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');        //removing from local storage
    this.currentUserSource.next(null);
  }
  getDecodedToken(token: string){
    return JSON.parse(atob(token.split('.')[1]))
  }
}
