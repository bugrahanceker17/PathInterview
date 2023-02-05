# 
# Path Mülakat Proje Dokümanı

- #### Kullanılan kütüphaneler : StackExchangeRedis, AutoMapper, Newtonsoft.Json, EntityFrameworkCore, DependencyInjection, JwtBearer

- Redis için localhost:6379 port'u kullanılmaktadır.

- Redis kurulumu için [bu kaynaktan](https://github.com/microsoftarchive/redis/releases) redis server indirilmiştir.

- Proje çalışmadan önce local veritabanına (MsSql) `PathInterviewDb` adında bir veritabanı oluşturulması önerilir.

- Proje ilk çalıştığında auto migration işlemi yapılacak ve ilgili tablolar doldurulacaktır.

***Verilen proje isterleri için tüm endpoint'lerin açıklaması aşağıda mevcuttur*** :


## Yeni Kullanıcı Oluştur

```http
  POST /api/Auth/register
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `fistname` | `string` | Yeni kullanıcı adı |
| `lastName` | `string` | Yeni kullanıcı soyadı |
| `email` | `string` | Yeni kullanıcı mail adresi |
| `password` | `string` | Yeni kullanıcı şifresi |


## Kullanıcı Girişi

```http
  POST /api/Auth/login
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `email` | `string` | Kullanıcı mail adresi |
| `password` | `string` | Kullanıcı şifresi |

#### Açıklama
- Eğer mail adresi ve parola doğru ise response modelde token döner.

## Ürün Listeleme

```http
  GET /api/Product/list
```

#### Açıklama
- Herhangi bir parametresi yoktur. 
- Örnek proje olduğu için tüm ürünleri listeler.

## Müşteri Sepet Listesi

```http
  GET /api/Basket
```


#### Açıklama

- Token ile işlem yapılmaktadır ve token zorunludur.
- Herhangi bir parametresi bulunmuyor.

## Sepete Ürün Ekleme

```http
  POST /api/Basket
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `productId` | `int` | Sepete eklenecek ürünün ID'si |
| `quantity` | `int` | Belirtilen ürünün sepete kaç adet ekleneceği |

#### Açıklama
- Sepete eklenecek ürünler kişi bazlı olduğu için token zorunludur.

## Sipariş Oluştur

```http
  POST /api/Order/add
```

#### Açıklama
- Token zorunludur.
- Herhangi bir parametresi yoktur.
- Sepete eklenen ürünlerin hepsini sipariş olarak kaydeder.
- Kaydedilen sipariş sonrasında sepet temizlenir.

## Sipariş Listeleme

```http
  GET /api/Order/list
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `page` | `int` | Sayfa numarası (Varsayılan = 1) |
| `pageSize` | `int` | Sayfa boyutu (Varsayılan = 20) |

#### Açıklama
- Token zorunludur.
- Daha önceden verilen siparişleri listeler.

## Müşteri Sipariş İptal

```http
  DELETE /api/Order/cancel
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `orderId` | `string` | Sipariş listesinde verilen orderId değeri |
| `productId` | `int` |Sipariş içindeki productId değeri |

#### Açıklama
- Token zorunludur.
- Sipariş içinde bulunan ürünleri iptal etmek için kullanılır.



## Yönetici Sipariş İptal İstekleri

```http
  GET /api/Order/cancel-requests
```

#### Açıklama
- Gıda kategorisinde olan ürünlerin iptal istekleri listeye gelir.
- Üst yöneticilerin görebileceği iptal istekleridir.

## Yönetici Sipariş İptal Onay ve Reddetme

```http
  POST /api/Order/confirm-cancel-request
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `id` | `int` | İşlem yapılacak sipariş ID'si |
| `isConfirm` | `bool` | İptal isteğini onaylama veya reddetme |

#### Açıklama
- Sipariş iptal işleminin yönetici tarafından onaylama veya reddetme işlemi yapar.
- 'isConfirm' değer `true` olması durumunda iptal isteği onaylanır.
