
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  // users : any;                                                       //__

  constructor(private accountService: AccountService){      //In the constructor, HttpClient is injected as a dependency. (Dependenct injection)

  }
  ngOnInit(): void {
   // throw new Error('Method not implemented.');
    // this.getUsers();                                                 __
    this.setCurrentUser();
  }

  // getUsers(){                                                                  //removed  __
  //   this.http.get('https://localhost:5001/api/Users').subscribe({
  //     //callback functions
  //     next: Response => this.users = Response,
  //     //error: () => {},
  //     error: error => console.log(error),
  //     complete: () => console.log('Request has completed')
  //    })
  // }   //to observe an  observable need to subscribe it

  setCurrentUser(){
   // const user : User = JSON.parse(localStorage.getItem('user')!);
   const userString = localStorage.getItem('user');
   if(!userString) return;
   const user:User = JSON.parse(userString);
   this.accountService.setCurrentUser(user);
  }
}
