function SetCartCookie(cart) {
    var now = new Date();
    now.setFullYear(now.getFullYear() + 1);

    var CartCookie = "cart=";

    var products = cart.map(item => item.productID + ":" + item.quantity);
    CartCookie += products.join(",");

    document.cookie = CartCookie + "; expires=" + now.toUTCString() + "; path=/";
}

function GetCart() {
    var CartCookie = document.cookie.match(/cart=([^;]+)/);

    if (CartCookie) {
        var cart = CartCookie[1].split(",").map(item => {
            const [productID, quantity] = item.split(":");
            return { productID: parseInt(productID), quantity: parseInt(quantity) };
        });

        return cart;
    }
    return [];
}

function AddToCart(productID) {
    var cart = GetCart();
    var isset = false;

    cart.forEach(item => {
        if (item.productID === productID) {
            item.quantity += 1;
            isset = true;
            document.getElementById("quantity[" + item.productID + "]").innerHTML = item.quantity;
        }
    });

    if (!isset) {
        cart.push({ productID: productID, quantity: 1 });
    }

    SetCartCookie(cart);

    DisplayCart();

    console.log(cart);
}

function DecreaseQuantity(productID) {
    var cart = GetCart();
        
    cart.forEach(item => {
        if (item.productID === productID) {
            if (item.quantity > 1) {
                item.quantity -= 1;
                document.getElementById("quantity[" + item.productID + "]").innerHTML = item.quantity;

            } else {
                RemoveFromCart(productID);
            }
        }
    });

    SetCartCookie(cart);
    DisplayCart();
    console.log(cart);
}

function RemoveFromCart(productID) {
    var cart = GetCart();

    var index = cart.findIndex(item => item.productID === productID);

    if (index !== -1) {
        cart.splice(index, 1);
    }

    SetCartCookie(cart);
    DisplayCart();

    console.log(cart);
}

function EmptyCart() {
    var cart = [];
    SetCartCookie(cart);
    console.log(cart);
    DisplayCart();
}


function DisplayCart() {
    var cart = GetCart();

    var Total=0;
    var cartItemsContainer = document.getElementById("cart-items");
    var cartContainer = document.getElementById("cart-container");
    var cartTotal = document.getElementById("cart-total");

    cartItemsContainer.innerHTML = "";

    if (cart.length === 0) {
        cartContainer.innerHTML = `<div class="container my-4 py-4">
            <div class="row my-4 py-5 justify-content-center">
                <div class="col-5">
                    <div class="alert align-items-center p-5" role="alert" style="background-color: #F0F0F3; box-shadow: 0px 0px 3px 3px #e9ecef;">
                        <div class="d-flex justify-content-center">
                            <i class="fa-solid fa-cart-shopping fa-7x"></i>
                        </div>
                        <h2 class="fs-4 text-center mt-5 fw-bold"> SEPETİNİZDE ÜRÜN YOK </h2>
                        <div class="d-flex justify-content-center mt-4">
                            <a href="/" class="btn btn-outline-dark border border-dark border-3 py-2 px-4 fs-5 fw-bold">ALIŞVERİŞE DEVAM ET</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        `;
    }
    else {

        cart.forEach(item => {
            $.ajax({
                url: '/Main/GetProductJson',
                type: 'GET',
                data: {
                    ProductID: item.productID
                },
                success: function (result) {

                    const product = `
                      <div class="col-12 col-xxl-6 p-0 pe-3">
                        <div class="card mb-4 w-100 d-inline-block rounded-3" style="height: 150px;">
                            <div class="container-fluid h-100">
                                <div class="row h-100">
                                    <div class="col-md-3 d-flex h-100 p-0">
                                        <img src="/Content/Images/ProductImages/`+result.Id+`/1.jpg" class="rounded-start m-auto w-75">
                                    </div>
                                    <div class="col-md-6">
                                        <div class="card-body p-0 d-flex flex-column h-100">
                                            <h5 class="card-title mt-3">`+ result.Name + `</h5>
                                            <p class="fs-2 mt-auto fw-bold lh-1">`+ result.Price.toLocaleString()+`TL</p>
                                        </div>
                                    </div>
                                    <div class="col-md-3 p-0 py-2 pe-2 h-100" style="z-index: 2;">
                                      <div class="container-fluid h-100">
                                        <div class="row h-50 bg-dark rounded-3">
                                            <div class="col-4 d-flex p-0">
                                                <button class="btn btn-outline-light fw-bold rounded-circle m-auto py-0 fs-4" onclick="DecreaseQuantity(`+ item.productID +`)">-</button>
                                            </div>
                                            <div class="col-4 d-flex p-0">
                                                <div class="m-auto fs-3 text-white fw-bold" id="quantity[`+item.productID+`]" >`+item.quantity+`</div>
                                            </div>
                                            <div class="col-4 d-flex p-0">
                                                <button class="btn btn-outline-light fw-bold rounded-circle m-auto py-0 fs-4" onclick="AddToCart(`+ item.productID +`)">+</button>
                                            </div>
                                        </div>
                                        <div class="row h-50">
                                            <div class="col-12 p-0 d-flex">
                                                <button class="btn btn-outline-dark m-auto fs-4 fw-bold w-100" onclick="RemoveFromCart(`+item.productID+`)">Sil <i class="fa-solid fa-trash-can fa-xs"></i></button>
                                            </div>
                                        </div>
                                      </div>  
                                    </div>
                                </div>
                            </div>
                            <a href="/urun?id=`+ result.Id +`" class="stretched-link"></a>
                        </div>
                      </div>`;
                    cartItemsContainer.innerHTML += product;
                    Total += result.Price * item.quantity;
                    cartTotal.innerHTML = Total.toLocaleString() + " TL";
                },
                error: function (xhr, status, error) {
                    console.log("AJAX hatası: " + error);
                    alert("Bir hata oluştu!");
                }
            });

        });

    }
}

DisplayCart();