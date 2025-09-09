import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuscaFormComponent } from './busca-form.component';

describe('BuscaFormComponent', () => {
  let component: BuscaFormComponent;
  let fixture: ComponentFixture<BuscaFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BuscaFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BuscaFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
