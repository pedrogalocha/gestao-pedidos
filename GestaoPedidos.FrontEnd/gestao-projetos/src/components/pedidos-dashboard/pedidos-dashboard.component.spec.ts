import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PedidosDashboardComponent } from './pedidos-dashboard.component';

describe('PedidosDashboardComponent', () => {
  let component: PedidosDashboardComponent;
  let fixture: ComponentFixture<PedidosDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PedidosDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PedidosDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
