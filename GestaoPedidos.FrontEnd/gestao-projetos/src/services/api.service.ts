import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

// Interfaces para tipar as respostas da API
export interface ItemPedido {
  id: number;
  produto: string;
  quantidade: number;
  precoUnitario: number;
}

export interface Pedido {
  clienteId: number;
  itens: ItemPedido[];
  precoTotal: number;
  status: string | null;
  dataCriacao: string;
  codigoPedido: number;
}

export interface ValorTotalResponse {
  pedidoId: number;
  valorTotal: number;
}

export interface QuantidadeResponse {
  clienteId: number;
  quantidadePedidos: number;
}

export interface PedidoRequest {
  clienteId: number;
  itens: { produto: string; quantidade: number; precoUnitario: number; }[];
}

export interface Cliente {
  id: number;
  nome: string;
  dataCadastro: string;
}

export interface PedidoDetalhado {
  codigoPedido: number;
  quantidadeItens: number;
  valorTotal: number;
  nomeCliente: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getValorTotalPedido(pedidoId: number): Observable<ValorTotalResponse> {
    return this.http.get<ValorTotalResponse>(`${this.apiUrl}/Pedidos/${pedidoId}/valor-total`);
  }

  getQuantidadePedidosPorCliente(clienteId: number): Observable<QuantidadeResponse> {
    return this.http.get<QuantidadeResponse>(`${this.apiUrl}/Pedidos/cliente/${clienteId}/quantidade`);
  }

  getPedidosPorCliente(clienteId: number): Observable<Pedido[]> {
    return this.http.get<Pedido[]>(`${this.apiUrl}/Pedidos/cliente/${clienteId}`);
  }

  getAllPedidos(): Observable<PedidoDetalhado[]> {
    return this.http.get<PedidoDetalhado[]>(`${this.apiUrl}/Pedidos`);
  }

  enfileirarPedido(pedido: PedidoRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/Pedidos`, pedido);
  }

  getAllClientes(): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(`${this.apiUrl}/Clientes`);
  }

  createClient(command: { nome: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/Clientes`, command);
  }
}