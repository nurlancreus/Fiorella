document.addEventListener("DOMContentLoaded", () => {
    let timeoutId = null;
    let productId = null;
    let stockCount = 0;

    const basketContainer = document.querySelector(".search-shopping .shopping");
    const productCard = document.querySelector(".product");
    const addProductContainer = document.querySelector(".product-details-add-basket");

    const initBasketEvents = () => {
        const emptyBasketOutput = document.querySelector(".basketAlert");
        const basketList = document.querySelector(".basketList");

        const showBasket = () => {
            if (emptyBasketOutput) {
                emptyBasketOutput.style.opacity = "100%";
            } else if (basketList) {
                basketList.style.height = "220px";
            }
        };

        const hideBasket = () => {
            if (emptyBasketOutput) {
                emptyBasketOutput.style.opacity = "0";
            } else if (basketList) {
                basketList.style.height = "0px";
            }
        };

        const removeBasketItem = (e) => {
            const clickedDeleteBtn = e.target.closest(".btn-product-delete");
            console.log(clickedDeleteBtn);
            if (clickedDeleteBtn) {
                productId = clickedDeleteBtn.dataset.id;
                removeFromBasket();
            }
        }

        basketContainer.addEventListener("mouseover", showBasket);
        basketContainer.addEventListener("mouseout", hideBasket);
        basketContainer.parentElement.addEventListener("click", removeBasketItem)
    };

    const initAddProductEvents = () => {
        const decQuantity = addProductContainer.querySelector(".counter .minus");
        const counting = addProductContainer.querySelector(".counter .counting");
        const incQuantity = addProductContainer.querySelector(".counter .plus");
        const addBasket = addProductContainer.querySelector(".addToBasket");

        let quantityCount = 0;
        stockCount = parseInt(addProductContainer.dataset.stock, 10);
        productId = addProductContainer.dataset.id;

        const updateQuantity = (operation) => {
            if (operation === "dec" && quantityCount > 0) quantityCount--;
            if (operation === "inc" && quantityCount < stockCount) quantityCount++;
            counting.textContent = quantityCount;
        };

        const handleButtonClick = (e) => {
            const clickedBtn = e.target.closest(".minus, .plus, .addToBasket");
            console.log(clickedBtn, e.target)
            if (!clickedBtn) return;

            if (clickedBtn === decQuantity) {
                updateQuantity("dec");
            } else if (clickedBtn === incQuantity) {
                updateQuantity("inc");
            } else if (clickedBtn === addBasket) {

                if (quantityCount == 0) {
                    showMessage("Please, specify the quantity first.", "error");
                    return;
                }
                console.log(quantityCount, productId);
                addToBasket(quantityCount, true);
            }
        };

        addProductContainer.addEventListener("click", handleButtonClick);
    };

    const initProductCardEvents = () => {
        document.body.addEventListener("click", (event) => {
            const clickedAddToCart = event.target.closest(".addToCardBtn");
            if (clickedAddToCart) {
                productId = clickedAddToCart.dataset.id;
                stockCount = parseInt(clickedAddToCart.dataset.stock, 10);

                if (stockCount <= 0) return;

                addToBasket(1);
            }
        });
    };

    const removeFromBasket = (show = false) => {
        const fetchOptions = { method: "DELETE" };

        fetch(`https://localhost:7045/product/removebasketitem/${productId}`, fetchOptions)
            .then(response => {
                if (!response.ok) throw new Error('Network response was not ok.');
                return response.json();
            })
            .then(data => {
                show && showMessage("Product removed from the basket successfully", "success");
                console.log("Product removed from the basket:", data);
            })
            .catch(error => {
                show && showMessage(`Error removed from the basket: ${error.message}`, "error");
                console.error("Error removed from the basket:", error);
            });
    };

    const addToBasket = (quantity, show = false) => {
        console.log("trigger");
        const fetchOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ id: productId, quantity })
        };

        fetch('https://localhost:7045/product/addbasketitem', fetchOptions)
            .then(response => {
                if (!response.ok) throw new Error('Network response was not ok.');
                return response.json();
            })
            .then(data => {
                show && showMessage("Product added to the basket successfully", "success");
                console.log("Product added to basket:", data);
            })
            .catch(error => {
                show && showMessage(`Error adding product to basket: ${error.message}`, "error");
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

    if (basketContainer) initBasketEvents();
    if (addProductContainer) initAddProductEvents();
    if (productCard) initProductCardEvents();
});
