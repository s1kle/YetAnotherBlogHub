import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SigninRedirectComponent } from './signin-redirect.component';

describe('SigninRedirectComponent', () => {
  let component: SigninRedirectComponent;
  let fixture: ComponentFixture<SigninRedirectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SigninRedirectComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SigninRedirectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
