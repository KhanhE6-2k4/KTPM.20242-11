/// Update the total quantity of products in the cart
function updateCartTotalQty() {
    fetch('/Cart/CartCount')
    .then(response => response.json())
    .then(data => {
        const count = data.count ?? 0;
        document.getElementById("cart-count").innerText = count;
    });
}

// Calculate total amount of coressponding product in the cart
function updateCartTotalAmount() {
    fetch('/Cart/AmountCount')
    .then(response => response.json())
    .then(data => {
        const amount = data.amount ?? 0;
        document.getElementById("cart-total-amount").innerText = amount.toLocaleString('vi-Vn') + ' VND';
    });
}

// Update when the page is reloaded
document.addEventListener("DOMContentLoaded", () => {
    updateCartTotalQty();
    updateCartTotalAmount();
})

// Update the cart when remove a product (Use event delegation)
function removeFromCart(productId) {
    fetch(`/Cart/RemoveFromCart?id=${productId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            const row = document.querySelector(`[data-id='${productId}']`).closest("tr");
            if (row) row.remove();
            // Update Total Amount
            updateCartTotalAmount();
            // Update Total Product Qty
            updateCartTotalQty();

        }
        else {
            alert(data.message || "Has error when removing prodcut.");
        }
    })
    .catch(error => {
        console.error("Error: ", error);
    });
}

document.addEventListener("click", (e) => {
    // check if this button has been clicked
    if (e.target.closest(".btn-remove")) {
        const id =  e.target.closest(".btn-remove").getAttribute("data-id");
        removeFromCart(id);
    } 
})

// // Update the cart when decrease or increase the quantity of a product
// function updateProductQty(productId, action) {
//     fetch(`/Cart/UpdateProductQty`, {
//         method: "POST",
//         headers: {
//             'Content-Type': 'application/json',
//             'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
//         },
//         body: JSON.stringify({ id: productId, action: action })
//     })
//     .then(response => response.json())
//     .then(data => {
//         if (data.success) {
//             // const input = document.querySelector(`input[data-id='${productId}']`);
//             // input.value = data.newQty;
//             const amount_result = document.querySelector(`[data-id='cart-product-amount-${productId}']`);
//             if (amount_result) {
//                 amount_result.textContent = `${data.newAmount.toLocaleString()} VND`;
//             }
//             updateCartTotalAmount();
//         }
//         else {
//             alert(data.message || "Has error when updating the quantity of this product");
//         }
//     })
//     .catch(error => {
//         console.error("Error: ", error);
//     });
// }

// document.addEventListener("click", (e) => {
//     // check if this button has been clicked
//     const btn = e.target.closest("button[data-action]");
//     if (!btn) return;
//     const action = btn.getAttribute("data-action");
//     const id = btn.getAttribute("data-id");
//     updateProductQty(id, action);
// })



// Update the cart when the user adds a product to the cart (Use onclick)
// function addToCart(productId, qty = 1) {
//     fetch(`/Cart/AddToCart?id=${productId}&qty=${qty}`, {
//         method: 'POST',
//         headers: {
//             'Content-Type': 'application/json',
//             'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
//         },
//     })
//     .then(response => response.json())
//     .then(data => {
//         if (data.success) {
//             document.getElementById("cart-count").innerText = data.cartItemCount;

//             const span = document.getElementById("cart-count");
//             span.classList.add("animate_animated", "animate_bounceIn");
//             setTimeout(() => {
//                 span.classList.remove("animate_animated", "animate_bounceIn");
//             }, 1000);
//         }
//         else {
//             alert(data.message || "Has Error when adding product.");
//         }
//     })
//     .catch(error => {
//         console.error("Error: ", error);
//     });
// }

// Update the cart when the user adds a product to the cart
function addToCart(productId, qty) {
    fetch(`/Cart/AddToCart?id=${productId}&qty=${qty}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            document.getElementById("cart-count").innerText = data.cartItemCount;

            const span = document.getElementById("cart-count");
            span.classList.add("animate_animated", "animate_bounceIn");
            setTimeout(() => {
                span.classList.remove("animate_animated", "animate_bounceIn");
            }, 1000);
        }
        else {
            alert(data.message || "Has Error when adding product.");
        }
    })
    .catch(error => {
        console.error("Error: ", error);
    });
}


function changeQty(productId, delta) {
    const input = document.getElementById(`qty-${productId}`);
    const max = parseInt(input.getAttribute("data-max"));
    let current = parseInt(input.value) || 1;

    current += delta;

    if (current < 1) current = 1;
    if (current > max) current = max;

    input.value = current;
    updateQtyButtons(productId);
}

function validateQty(productId) {
    const input = document.getElementById(`qty-${productId}`);
    const max = parseInt(input.getAttribute("data-max"));
    let val = parseInt(input.value);

    if (isNaN(val) || val < 1) val = 1;
    if (val > max) val = max;

    input.value = val;
    updateQtyButtons(productId);
}

function updateQtyButtons(productId) {
    const input = document.getElementById(`qty-${productId}`);
    const minusBtn = input.parentElement.querySelector(".btn-minus");
    const plusBtn = input.parentElement.querySelector(".btn-plus");
    const val = parseInt(input.value);
    const max = parseInt(input.getAttribute("data-max"));

    minusBtn.disabled = val <= 1;
    plusBtn.disabled = val >= max;
}


// Gửi khi click +/- button
function updateProductQty(productId, action) {
    const input = document.getElementById(`qty-${productId}`);
    let currentQty = parseInt(input.value) || 1;
    const maxQty = parseInt(input.getAttribute("data-max"));

    if ((action === "plus" && currentQty >= maxQty) || 
        (action === "minus" && currentQty <= 1)) {
        return;
    }

    fetch(`/Cart/UpdateProductQty`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        body: JSON.stringify({ id: productId, action: action })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            input.value = data.newQty;
            updateQtyButtons(productId);
            const amount_result = document.querySelector(`[data-id='cart-product-amount-${productId}']`);
            if (amount_result) {
                amount_result.textContent = `${data.newAmount.toLocaleString()} VND`;
            }
            updateCartTotalAmount();
        } else {
            alert(data.message || "Error updating quantity");
        }
    })
    .catch(error => {
        console.error("Error: ", error);
    });
}

// Gửi khi người dùng nhập tay (blur)
function submitQty(productId) {
    const input = document.getElementById(`qty-${productId}`);
    let val = parseInt(input.value) || 1;
    const max = parseInt(input.getAttribute("data-max"));

    if (val > max) val = max;
    if (val < 1) val = 1;

    // Tạm thời có thể gọi như một update trực tiếp (nếu bạn muốn phân biệt thì sửa Controller để hỗ trợ action: "set")
    fetch(`/Cart/UpdateProductQty`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        body: JSON.stringify({ id: productId, action: "set", qty: val })
    })
    .then(res => res.json())
    .then(data => {
        if (data.success) {
            input.value = data.newQty;
            updateQtyButtons(productId);
            const amount_result = document.querySelector(`[data-id='cart-product-amount-${productId}']`);
            if (amount_result) {
                amount_result.textContent = `${data.newAmount.toLocaleString()} VND`;
            }
            updateCartTotalAmount();
        }
    });
}

// Gắn event click cho nút + -
document.addEventListener("click", (e) => {
    const btn = e.target.closest("button[data-action]");
    if (!btn) return;
    const action = btn.getAttribute("data-action");
    const id = btn.getAttribute("data-id");
    updateProductQty(id, action);
});