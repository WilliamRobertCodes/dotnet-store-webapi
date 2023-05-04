export function formatPriceInCents(priceInCents) {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
    }).format(priceInCents / 100);
}
