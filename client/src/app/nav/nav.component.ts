import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

model : any =  {};
// loggedIn = false;
// currentUser$ : Observable<User | null> = of(null);           // removed and directly used

  constructor(public accountService: AccountService) { }   //make public

  ngOnInit(): void {
   // this.getCurrentUser();
  //  this.currentUser$ = this.accountService.currentUser$;     //directly used
  }

  // getCurrentUser(){
  //   this.accountService.currentUser$.subscribe({
  //     next: user => this.loggedIn = !!user,                       
  //     error: error => console.log(error)
  //   })
  // }

  login(){
    this.accountService.login(this.model).subscribe({
        next: response => {
        console.log(response);
       // console.log("this is called");
        //this.loggedIn = true;

      },
      error: error => console.log(error)
      
    })
    // console.log(this.model);
  }
  logout(){
   // this.loggedIn = false;
    this.accountService.logout();         //remove item from local storage
  }
 
}
