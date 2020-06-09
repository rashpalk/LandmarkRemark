import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MouseEvent } from '@agm/core';
import { UserService } from 'src/app/services/UserService';
import { UserModel } from './models/UserModel';
import { UsersDataService } from './services/UsersDataService';
import { UsersDataModel } from './models/UsersDataModel';
import { Subscription } from 'rxjs';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {


  private subscriptions: Subscription[] = [];
  public zoom: number = 8;
  public registrationForm: FormGroup;
  public notesForm: FormGroup;
  public notesSearchForm: FormGroup;
  public message: string;
  public messageNote: string;
  private userModel: UserModel = <UserModel>{};
  public IsUserLoggedIn: boolean = false;
  public newUser: boolean = false;
  public loggedInUserName: string;
  public buttonTitle: string = "Login";
  public formTitle: string = "Sign In";
  public markers: marker[] = [];
  private usersDataModel: UsersDataModel = <UsersDataModel>{};
  public searchNotesResult: UsersDataModel[] = [];
  public submitted = false;
  public notesSubmitted = false;
  public isLocationChanged = false;
  public lat: number;
  public lng: number;
  public minClusterSize = 50;

  //initializing the  services & classes
  constructor(private titleService: Title, private userService: UserService, private usersDataService: UsersDataService, private formBuilder: FormBuilder) {

  }

  ngOnInit() {
    this.InitializeForm();
  }

  //initializing the forms
  private InitializeForm() {

    this.titleService.setTitle("Landmark Remark");

    this.registrationForm = this.formBuilder.group({
      firstName: [''],
      lastName: [''],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmpassword: ['']
    }
    );

    this.notesForm = this.formBuilder.group({
      labelValue: ['', Validators.required],
      isPublic: [false],
    });

    this.notesSearchForm = this.formBuilder.group({
      searchName: [''],
      searchString: [''],
    });


  }
  //shorthand for getter  form controls
  get f() { return this.registrationForm.controls; }
  get n() { return this.notesForm.controls; }
  get ns() { return this.notesSearchForm.controls; }

  //same function used for registering new users and logging in the existing users
  public SubmitClicked() {

    this.submitted = true;
    if (this.registrationForm.invalid) {
      return
    }
    else {
      if (this.newUser == true) {
        this.AddNewUser();
      }
      else {
        this.LoginUser();
      }
    }

  }
  //initializing the map here from database with all public notes saved by other users and all notes for currently logged in user

  //new user registration
  private AddNewUser() {

    this.userModel.firstName = this.registrationForm.controls.firstName.value;
    this.userModel.lastName = this.registrationForm.controls.lastName.value;
    this.userModel.userName = this.registrationForm.controls.email.value;
    this.userModel.password = this.registrationForm.controls.password.value;
    this.subscriptions.push(this.userService.RegisterUser(this.userModel).subscribe((response: number) => {
      console.log(response);
      if (this.userModel.userId == null && response != -1) {
        this.message = "Registered successfully";
        this.loggedInUserName = this.userModel.firstName + " " + this.userModel.lastName;
        this.ClearControls();
        this.InitializeMap();
        this.IsUserLoggedIn = true;
        this.newUser = false;
      }
      else if (this.userModel.userId != null && response != -1) {
        this.message = "Updated successfully";
        this.ClearControls();
        this.InitializeMap();
        this.IsUserLoggedIn = true;
        this.newUser = false;
      }
    }, err => {
      this.message = "error occoured while registering";
    }));
  }

  //existing user login
  private LoginUser() {
    this.userModel.userName = this.registrationForm.controls.email.value;
    this.userModel.password = this.registrationForm.controls.password.value;
    this.subscriptions.push(this.userService.GetUser(null, this.userModel.userName, this.userModel.password).subscribe((response) => {
      console.log(response);
      if (response.userId != null) {
        this.userModel = response;
        this.loggedInUserName = this.userModel.firstName + " " + this.userModel.lastName;
        this.InitializeMap();
        this.IsUserLoggedIn = true;
        this.ClearControls();
      }
      else {
        this.message = "User does not exist";
      }
    }, err => {
      this.message = "error occoured while logging in";
    }));
  }
  private InitializeMap() {
    if (navigator) {

      this.subscriptions.push(this.usersDataService.GetUsersData(this.userModel.userId, null, null).subscribe(
        (response) => {
          console.log(response);
          this.markers = [];
          response.forEach(element => {
            this.markers.push(
              {
                latitude: Number(element.latitude),
                longitude: Number(element.longitude),
                label: element.label,
                draggable: true
              }
            )
          });
        },
        err => {
          this.message = "error occoured";
        }
      ));
      console.log(navigator);
      this.SerCurrentLocationOnMap();
    }
  }
  //clearing the input controls here 
  private ClearControls() {
    this.message = "";
    this.registrationForm.patchValue({
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmpassword: ''
    });
  }

  //on click of not registered 
  public NewUserClicked() {
    let validator: Validators;
    this.newUser = true;
    this.formTitle = "Registration"
    this.buttonTitle = "Register";
    this.message = "";
    this.submitted = false;
    this.userModel = <UserModel>{};
    this.ClearControls();

    //dynamically adding validations
    this.registrationForm.controls.firstName.setValidators([Validators.required]);
    this.registrationForm.controls.firstName.updateValueAndValidity({ emitEvent: false, onlySelf: true });
    this.registrationForm.controls.lastName.setValidators([Validators.required]);
    this.registrationForm.controls.lastName.updateValueAndValidity({ emitEvent: false, onlySelf: true });
    this.registrationForm.controls.confirmpassword.setValidators([Validators.required, Validators.minLength(6)]);
    this.registrationForm.controls.confirmpassword.updateValueAndValidity({ emitEvent: false, onlySelf: true });
  }

  //onclick of sign in button to bring the user back to sign in form
  public SignInClicked() {
    this.newUser = false;
    this.formTitle = "Sign In"
    this.buttonTitle = "Login";
    this.ClearControls();
    this.submitted = false;
    this.registrationForm.controls.firstName.setValidators(null);
    this.registrationForm.controls.firstName.updateValueAndValidity({ emitEvent: false, onlySelf: true });
    this.registrationForm.controls.lastName.setValidators(null);
    this.registrationForm.controls.lastName.updateValueAndValidity({ emitEvent: false, onlySelf: true });
    this.registrationForm.controls.confirmpassword.setValidators(null);
    this.registrationForm.controls.confirmpassword.updateValueAndValidity({ emitEvent: false, onlySelf: true });

  }

  //seeting the current location on the map
  private SerCurrentLocationOnMap() {
    navigator.geolocation.getCurrentPosition(pos => {
      this.lng = pos.coords.longitude;
      this.lat = pos.coords.latitude;
    });
    this.isLocationChanged = false;
  }
  clickedMarker(label: string, index: number) {
    console.log(`clicked the marker: ${label || index}`)
  }

  //setting the longitude and latitude based on coordinatesclicked on map
  mapClicked($event: MouseEvent) {

    this.lat = $event.coords.lat;
    this.lng = $event.coords.lng;

    this.markers.push({
      latitude: $event.coords.lat,
      longitude: $event.coords.lng,
      label: "",
      draggable: true
    });
  }

  // on click of log out
  public LogOutClicked() {
    this.IsUserLoggedIn = false;
    this.newUser = false;
    this.submitted = false;
  }

  // #region Add Notes
  public SaveNotes() {
    this.notesSubmitted = true;
    if (this.notesForm.invalid) {
      return
    }
    else {
      this.usersDataModel.longitude = this.lng.toString();
      this.usersDataModel.latitude = this.lat.toString();
      this.usersDataModel.label = this.notesForm.controls.labelValue.value;
      this.usersDataModel.isPublic = this.notesForm.controls.isPublic.value;
      this.usersDataModel.userId = this.userModel.userId;

      this.subscriptions.push(this.usersDataService.SaveUserData(this.usersDataModel).subscribe(
        (response: number) => {
          console.log(response);
          if (this.usersDataModel.userDataId == null && response != -1) {
            this.messageNote = "note added successfully";
            this.notesForm.patchValue({
              labelValue: '',
              isPublic: false
            });
            this.loggedInUserName = this.userModel.firstName + " " + this.userModel.lastName;
            this.InitializeMap();
          }
          else if (this.usersDataModel.userDataId != null && response != -1) {
            this.message = "note updated successfully";
            this.notesForm.patchValue({
              labelValue: '',
              isPublic: false
            });
            this.InitializeMap();
          }
        },
        err => {
          this.message = "error occoured while saving note";
        }
      ));
      this.notesSubmitted = false;
    }

  }

  public trackByFnNotes(index, item) {
    if (!item)
      return null;
    return item.id;
  }
  // #endregion

  // #region Search note
  //on click of search note button
  public SearchNoteClicked() {

    this.subscriptions.push(this.usersDataService.GetUsersData(null, this.notesSearchForm.controls.searchName.value == "" ? null : this.notesSearchForm.controls.searchName.value, this.notesSearchForm.controls.searchString.value == "" ? null : this.notesSearchForm.controls.searchString.value).subscribe(
      (response) => {
        console.log(response);
        this.searchNotesResult = response;
        console.log(this.searchNotesResult);
      },
      err => {
        this.message = "error occoured while fetching notes";
      }
    ));
  }
  public ViewOnMapClicked(note: UsersDataModel) {

    this.lat = Number(note.latitude);
    this.lng = Number(note.longitude);
    this.isLocationChanged = true;
  }
  //adding notes here



  public trackByFn(index, item) {
    if (!item)
      return null;
    return item.id;
  }

  public BaktoCurrentLocClicked() {
    this.SerCurrentLocationOnMap();
  }

  //unsubscibing the subscriptions
  public ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

}

//just an interface for type safety.
interface marker {
  latitude: number;
  longitude: number;
  label?: string;
  draggable: boolean;
}
