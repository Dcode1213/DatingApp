import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

@Injectable({           //decorator
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  
  private currentUserSource = new BehaviorSubject<User | null>(null);             //pending - an observable
  currentUser$ = this.currentUserSource.asObservable();                            //pending 

  constructor(private http: HttpClient) { }   

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
      map((response : User) =>{
        const user = response;
        if(user){
          localStorage.setItem('user',JSON.stringify(user))  //setting item to the local storage
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl +'account/register',model).pipe(
      map(user => {
        if(user)
        {
          localStorage.setItem('user',JSON.stringify(user))
          this.currentUserSource.next(user);
        }
        // return user;        //not want 
      })
    )
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');        //removing from local storage
    this.currentUserSource.next(null);
  }
}
