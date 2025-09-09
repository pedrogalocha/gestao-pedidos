import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Pedido, QuantidadeResponse, ValorTotalResponse } from '../../services/api.service';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-resultados',
  templateUrl: './resultados.component.html',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatListModule
  ]
})
export class ResultadosComponent {
  @Input() valorTotalPedido: ValorTotalResponse | null = null;
  @Input() quantidadePedidos: QuantidadeResponse | null = null;
  @Input() listaDePedidos: Pedido[] = [];
  @Input() loading = false;
  @Input() error: string | null = null;
}