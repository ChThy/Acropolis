const ellipsis = "...";

export default function truncate(text: string | undefined | null, maxLength: number) {
  if (!text || text.length <= maxLength) {
    return text;
  }
  return text.substring(0, maxLength - ellipsis.length) + ellipsis;
};
