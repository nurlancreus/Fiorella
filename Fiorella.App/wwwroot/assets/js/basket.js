document.addEventListener("DOMContentLoaded", () => {
    let timeoutId = null;  // Timeout ID stored here
    let productId = null;  // Product ID stored here
    let stockCount = 0;  // Product stock stored here

    const addProductContainer = document.querySelector(".product-details-add-basket");

    if (addProductContainer) {
        const decQuantity = addProductContainer.querySelector(".counter .minus");
        const counting = addProductContainer.querySelector(".counter .counting");
        const incQuantity = addProductContainer.querySelector(".counter .plus");
        const addBasket = addProductContainer.querySelector(".counter .addBasket");

        let quantityCount = 0;
        stockCount = parseInt(addProductContainer.dataset.stock, 10);
        productId = addProductContainer.dataset.id;

        const updateQuantity = (operation) => {
            if (operation === "dec" && quantityCount > 0) quantityCount--;
            if (operation === "inc" && quantityCount < stockCount) quantityCount++;
            counting.textContent = quantityCount;
        };

        const handleButtonClick = (e) => {
            const clickedBtn = e.target.closest(".counter .minus, .counter .plus, .counter .addBasket");
            if (!clickedBtn) return;

            if (clickedBtn === decQuantity) {
                updateQuantity("dec");
            } else if (clickedBtn === incQuantity) {
                updateQuantity("inc");
            } else if (clickedBtn === addBasket) {
                addToBasket(quantityCount, true);
            }
        };

        addProductContainer.addEventListener("click", handleButtonClick);
    } else {
        document.body.addEventListener("click", (event) => {
            const clickedAddToCart = event.target.closest(".addToCartBtn");
            if (!clickedAddToCart) return;

            productId = clickedAddToCart.dataset.id;
            stockCount = parseInt(clickedAddToCart.dataset.stock, 10);

            if (stockCount <= 0) return;

            addToBasket(1);  // Assuming default quantity to add is 1
        });
    }

    const removeFromBasket = () => {
        const fetchOptions = {
            method: "DELETE",
        };

        fetch(`/product/removebasketitem/${productId}`, fetchOptions)
            .then(response => {
                if (!response.ok) throw new Error('Network response was not ok.');
                return response.json();
            })
            .then(data => {
                showMessage("Product removed from the basket successfully", "success");
                console.log("Product removed from the basket:", data);
            })
            .catch(error => {
                showMessage(`Error removed from the basket: ${error.message}`, "error");
                console.error("Error removed from the basket:", error);
            });
    }

    const addToBasket = (quantity, show = false) => {
        const fetchOptions = {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ quantity })
        };

        fetch(`/product/addbasket/${productId}`, fetchOptions)
            .then(response => {
                if (!response.ok) throw new Error('Network response was not ok.');
                return response.json();
            })
            .then(data => {
                showMessage("Product added to the basket successfully", "success");
                console.log("Product added to basket:", data);
            })
            .catch(error => {
                showMessage(`Error adding product to basket: ${error.message}`, "error");
                console.error("Error adding product to basket:", error);
            });
    };

    const showMessage = (message, type, ms = 3000) => {
        const messageContainer = document.querySelector(".basket-message");
        messageContainer.classList.remove("text-danger", "text-success", "opacity-0", "visually-hidden");
        messageContainer.classList.add(type === "error" ? "text-danger" : "text-success");
        messageContainer.textContent = message;

        if (timeoutId) {
            clearTimeout(timeoutId);
        }

        timeoutId = setTimeout(() => {
            messageContainer.classList.add("opacity-0", "visually-hidden");
        }, ms);
    };
});
